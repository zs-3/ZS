using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SmallBasic.Library;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Web;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using ZS;

namespace ZS
{

	/// <summary>
	/// Network related functions.
	/// </summary>
	[SmallBasicType]
	public static class ZSNetwork
	{
		private static HttpClient client = new HttpClient();

		/// <summary>
		/// Sends a GET request to the specified URL.
		/// </summary>
		/// <param name="url">The URL to send the GET request to.</param>
		/// <returns>The response content as a string.</returns>
		public static Primitive Get(Primitive url)
		{
			try {
				Task<string> task = client.GetStringAsync(url.ToString());
				task.Wait();
				return task.Result;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Sends a POST request to the specified URL with the given data.
		/// </summary>
		/// <param name="url">The URL to send the POST request to.</param>
		/// <param name="data">The data to send in the body of the request.</param>
		/// <returns>The response content as a string.</returns>
		public static Primitive Post(Primitive url, Primitive data)
		{
			try {
				StringContent content = new StringContent(data.ToString(), System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
				Task<HttpResponseMessage> task = client.PostAsync(url.ToString(), content);
				task.Wait();
				Task<string> responseTask = task.Result.Content.ReadAsStringAsync();
				responseTask.Wait();
				return responseTask.Result;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Downloads a file from a given URL to the specified local path.
		/// </summary>
		/// <param name="url">The file URL.</param>
		/// <param name="filePath">The local file path to save to.</param>
		/// <returns>Success or error message.</returns>
		public static Primitive DownloadFile(Primitive url, Primitive filePath)
		{
			try {
				using (WebClient wc = new WebClient()) {
					wc.DownloadFile(url.ToString(), filePath.ToString());
					return "Download successful!";
				}
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Gets the public IP address of the current machine.
		/// </summary>
		/// <returns>The public IP address as a string.</returns>
		public static Primitive GetPublicIP()
		{
			try {
				using (WebClient wc = new WebClient()) {
					return wc.DownloadString("https://api.ipify.org");
				}
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Resolves a domain name to its IP address.
		/// </summary>
		/// <param name="domain">The domain name to resolve.</param>
		/// <returns>The IP address of the domain.</returns>
		public static Primitive DnsResolve(Primitive domain)
		{
			try {
				IPAddress[] addresses = Dns.GetHostAddresses(domain.ToString());
				return addresses.Length > 0 ? addresses[0].ToString() : "No IP found";
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Checks if the internet connection is available.
		/// </summary>
		/// <returns>True if connected, otherwise false.</returns>
		public static Primitive IsInternetAvailable()
		{
			try {
				using (WebClient wc = new WebClient()) {
					wc.DownloadString("http://www.google.com");
					return true;
				}
			} catch {
				return false;
			}
		}
		
		/// <summary>
		/// Retrieves the headers from a given URL.
		/// </summary>
		/// <param name="url">The URL to fetch headers from.</param>
		/// <returns>The headers as a formatted string.</returns>
		public static Primitive GetHeaders(Primitive url)
		{
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.ToString());
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				WebHeaderCollection headers = response.Headers;
				response.Close();
				return headers.ToString();
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Checks if a URL exists and is reachable.
		/// </summary>
		/// <param name="url">The URL to check.</param>
		/// <returns>True if the URL exists, false otherwise.</returns>
		public static Primitive CheckUrlExists(Primitive url)
		{
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.ToString());
				request.Method = "HEAD";
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
					return response.StatusCode == HttpStatusCode.OK;
				}
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Retrieves the hostname of the local machine.
		/// </summary>
		/// <returns>The local machine's hostname.</returns>
		public static Primitive GetHostName()
		{
			try {
				return Dns.GetHostName();
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Sends a ping request to a host and returns the response time.
		/// </summary>
		/// <param name="host">The host to ping (IP or domain).</param>
		/// <returns>The response time in milliseconds or an error message.</returns>
		public static Primitive PingHost(Primitive host)
		{
			try {
				using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping()) {
					System.Net.NetworkInformation.PingReply reply = ping.Send(host.ToString());
					return reply.Status == System.Net.NetworkInformation.IPStatus.Success ? reply.RoundtripTime + "ms" : "Ping failed";
				}
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the local IP address of the machine.
		/// </summary>
		/// <returns>The local IP address.</returns>
		public static Primitive GetLocalIP()
		{
			try {
				string localIP = "Not found";
				foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName())) {
					if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
						localIP = ip.ToString();
						break;
					}
				}
				return localIP;
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Uploads a file to a specified server URL.
		/// </summary>
		/// <param name="url">The server URL to upload the file.</param>
		/// <param name="filePath">The local file path to upload.</param>
		/// <returns>The server response or an error message.</returns>
		public static Primitive UploadFile(Primitive url, Primitive filePath)
		{
			try {
				using (WebClient client = new WebClient()) {
					byte[] responseBytes = client.UploadFile(url.ToString(), filePath.ToString());
					return Encoding.UTF8.GetString(responseBytes);
				}
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Extracts meta tags from a webpage.
		/// </summary>
		/// <param name="url">The URL of the webpage.</param>
		/// <returns>A formatted string containing meta tag details.</returns>
		public static Primitive ExtractMetaTags(Primitive url)
		{
			try {
				using (WebClient client = new WebClient()) {
					string html = client.DownloadString(url.ToString());
					MatchCollection matches = Regex.Matches(html, "<meta\\s+(?:[^>]*?\\s+)?content=[\"']([^\"']+)[\"']", RegexOptions.IgnoreCase);
            
					StringBuilder metaTags = new StringBuilder();
					foreach (Match match in matches) {
						metaTags.AppendLine(match.Groups[1].Value);
					}
            
					return metaTags.ToString();
				}
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the MAC address of the local machine.
		/// </summary>
		/// <returns>The MAC address as a string.</returns>
		public static Primitive GetMacAddress()
		{
			try {
				foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
					if (nic.OperationalStatus == OperationalStatus.Up) {
						return BitConverter.ToString(nic.GetPhysicalAddress().GetAddressBytes());
					}
				}
				return "MAC Address not found";
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Checks if a specific port is open on a given IP address or domain.
		/// </summary>
		/// <param name="host">The IP or domain to check.</param>
		/// <param name="port">The port number to check.</param>
		/// <returns>True if the port is open, false otherwise.</returns>
		public static Primitive PortCheck(Primitive host, Primitive port)
		{
			try {
				using (TcpClient client = new TcpClient()) {
					IAsyncResult result = client.BeginConnect(host.ToString(), port, null, null);
					bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
					if (success) {
						client.EndConnect(result);
						return true;
					}
					return false;
				}
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Performs a traceroute to a given host and returns the route details.
		/// </summary>
		/// <param name="host">The destination host (IP or domain).</param>
		/// <returns>The traceroute results as a string.</returns>
		public static Primitive TraceRoute(Primitive host)
		{
			try {
				StringBuilder traceResult = new StringBuilder();
				using (Process p = new Process()) {
					p.StartInfo.FileName = "tracert";
					p.StartInfo.Arguments = host.ToString();
					p.StartInfo.UseShellExecute = false;
					p.StartInfo.RedirectStandardOutput = true;
					p.Start();
            
					while (!p.StandardOutput.EndOfStream) {
						traceResult.AppendLine(p.StandardOutput.ReadLine());
					}
					p.WaitForExit();
				}
				return traceResult.ToString();
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		
		
	}
}