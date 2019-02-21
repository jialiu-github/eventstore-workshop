using System.Text;
using Newtonsoft.Json;

namespace Eventstore_workshop
{
    public class Json
    {
        public static byte[] SerializeToBytes(object o)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));
        }

        public static TE DeserializeFromBytes<TE>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<TE>(Encoding.UTF8.GetString(bytes));
        }
    }
}