using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AutoUpdater.NETStandard
{
	public class UpdateInfo : EventArgs
	{
        /// <summary>
        ///     Application name retrieved from Assembly.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        ///     If new update is available then returns true otherwise false.
        /// </summary>
        public bool IsUpdateAvailable { get; set; }

        /// <summary>
        ///     Download URL of the update file.
        /// </summary>
        [JsonProperty("url")]
		public string DownloadURL { get; set; }

        /// <summary>
        ///     URL of the webpage specifying changes in the new update.
        /// </summary>
        [JsonProperty("changelog")]
		public string ChangelogURL { get; set; }

        /// <summary>
        ///     Returns newest version of the application available to download.
        /// </summary>
        [JsonProperty("version")]
		public string CurrentVersion { get; set; }

        /// <summary>
        ///     Returns version of the application currently installed on the user's PC.
        /// </summary>
        public string InstalledVersion { get; set; }

        /// <summary>
        ///     Checksum of the update file.
        /// </summary>
        [JsonProperty("checksum")]
		public CheckSum CheckSum { get; set; }

        /// <summary>
        ///     Defines behaviour of the update procedure.
        /// </summary>
        [JsonProperty("type")]
		public UpdateType Type { get; set; }

        /// <summary>
        ///     Command line arguments used by Installer.
        /// </summary>
        [JsonProperty("args")]
		public string InstallerArgs { get; set; }
	}

	public class CheckSum
	{
        /// <summary>
        ///     Hash of the file.
        /// </summary>
        [XmlText]
		public string Text { get; set; }

        /// <summary>
        ///     Hash algorithm that generated the hash.
        /// </summary>
        [XmlAttribute("algorithm")]
		public string HashingAlgorithm { get; set; }
	}
}
