﻿using System;
using System.IO;

namespace sonospodcastservice.HTTP
{
	public class SonosSMAPIServer : HttpServer {

		public SonosSMAPIServer(String ListeningIPAddress, int ListeningPort)
			: base(ListeningIPAddress,ListeningPort) 
		{
		}

		public override void handleGETRequest (HttpProcessor p)
		{
			/*
			if (p.http_url.Equals ("/Test.png")) {
				Stream fs = File.Open("../../Test.png",FileMode.Open);

				p.writeSuccess("image/png");
				fs.CopyTo (p.outputStream.BaseStream);
				p.outputStream.BaseStream.Flush ();
			}

			Console.WriteLine("request: {0}", p.http_url);
			p.writeSuccess();
			p.outputStream.WriteLine("<html><body><h1>test server</h1>");
			p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
			p.outputStream.WriteLine("url : {0}", p.http_url);

			p.outputStream.WriteLine("<form method=post action=/SMAPI>");
			p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
			p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
			p.outputStream.WriteLine("</form>");*/
			p.writeFailure ();
		}

		public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData) {
			Console.WriteLine("POST request: {0}", p.http_url);
			string data = inputData.ReadToEnd();

			/*Console.WriteLine (data);
			foreach(String Hashvalue in p.httpHeaders.Values)
			{
				Console.WriteLine (Hashvalue);
			}*/
			// this is where the POST requests are handled, regardless of the actual URL

			// step 1: find out which SMAPI method is called by taking a look at the SOAPACTION Header
			if (p.httpHeaders.ContainsKey("SOAPACTION"))
			{
				// we got a SOAPACTION key, now retrieve it
				String rawSOAPACTION = (String)p.httpHeaders ["SOAPACTION"];

				// the SOAPACTION header will be in this format:
				// SOAPACTION: "http://www.sonos.com/Services/1.1#$command"
				//
				// we only have to filter it for the #$command part
				String SOAPACTION = rawSOAPACTION.Remove (0, rawSOAPACTION.LastIndexOf ("#")+1);
				SOAPACTION = SOAPACTION.Remove(SOAPACTION.Length-1);

				Console.WriteLine (SOAPACTION.ToUpper ());

				String SMAPIAnswer = "";

				/*switch (SOAPACTION.ToUpper())
				{
				case "GETLASTUPDATE":
					Console.WriteLine ("GetLastUpdate called");
					SMAPIAnswer = SMAPI.GetLastUpdate (xsnCurrentData, data);
					break;
				case "GETMETADATA":
					Console.WriteLine("getMetadata called");
					SMAPIAnswer = SMAPI.getMetadata(xsnCurrentData, data);
					break;
				case "GETMEDIAMETADATA":
					Console.WriteLine ("getMediaMetadata called");
					SMAPIAnswer = SMAPI.getMediaMetadata (xsnCurrentData, data);
					break;
				case "GETMEDIAURI":
					Console.WriteLine ("getMediaURI called");
					SMAPIAnswer = SMAPI.getMediaURI (xsnCurrentData, data);
					break;
				default:
					Console.WriteLine ("Unknown: " + SOAPACTION);
					break;
				}*/

				if (SMAPIAnswer.Length > 0) {
					// we got an answer from the SMAPI handlers, pipe it out
					p.writeSuccess ("Content-Type: text/xml; charset=utf-8");
					p.outputStream.WriteLine (SMAPIAnswer);
				} else
					p.writeFailure ();

			}
		}
	}
}

