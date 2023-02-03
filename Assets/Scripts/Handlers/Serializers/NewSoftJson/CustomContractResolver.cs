using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Services.SerializerService.Serializers.NewSoftJson
{
    public class CustomContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo memberInfo, MemberSerialization memberSerialization)
        {
            return CreateAbilityReadPrivateProperty(memberInfo, memberSerialization);
        }

        private JsonProperty CreateAbilityReadPrivateProperty(MemberInfo memberInfo, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(memberInfo, memberSerialization);

            if (!jsonProperty.Writable)
            {
                var property = memberInfo as PropertyInfo;

                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    jsonProperty.Writable = hasPrivateSetter;
                }
            }

            return jsonProperty;
        }
    }
}