using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamOverlay.Classes.Twitch
{
    public class Twitch
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        public class Channel
        {
            public string id { get; set; }
            public Self self { get; set; }
            public Trailer trailer { get; set; }
            public Home home { get; set; }
            public string __typename { get; set; }
        }

        public class Data
        {
            public UserOrError userOrError { get; set; }
            public User user { get; set; }
        }

        public class Extensions
        {
            public int durationMilliseconds { get; set; }
            public string operationName { get; set; }
            public string requestID { get; set; }
        }

        public class Followers
        {
            public int totalCount { get; set; }
            public string __typename { get; set; }
        }

        public class Home
        {
            public Preferences preferences { get; set; }
            public string __typename { get; set; }
        }

        public class Preferences
        {
            public string heroPreset { get; set; }
            public string __typename { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
            public Extensions extensions { get; set; }
        }

        public class Self
        {
            public bool isAuthorized { get; set; }
            public object restrictionType { get; set; }
            public string __typename { get; set; }
        }

        public class Stream
        {
            public string id { get; set; }
            public int viewersCount { get; set; }
            public string __typename { get; set; }
            public DateTime createdAt { get; set; }
        }

        public class Trailer
        {
            public object video { get; set; }
            public string __typename { get; set; }
        }

        public class User
        {
            public string id { get; set; }
            public Followers followers { get; set; }
            public bool isPartner { get; set; }
            public object primaryColorHex { get; set; }
            public string __typename { get; set; }
            public string login { get; set; }
            public Stream stream { get; set; }
        }

        public class UserOrError
        {
            public string id { get; set; }
            public string login { get; set; }
            public string displayName { get; set; }
            public object primaryColorHex { get; set; }
            public string profileImageURL { get; set; }
            public Stream stream { get; set; }
            public string __typename { get; set; }
            public object bannerImageURL { get; set; }
            public Channel channel { get; set; }
        }

    }
}