using System;
using System.Reflection;

namespace System.Reflection
{
    class SR
    {
        public static string NullabilityInfoContext_NotSupported = "NullabilityInfoContext is not supported";
    }

    static class NullabilityInfoTypeExtensions
    {
        public static MemberInfo GetMemberWithSameMetadataDefinitionAs(this Type type, MemberInfo member)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));

            const BindingFlags all = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            foreach (MemberInfo myMemberInfo in type.GetMembers(all))
            {
                if (myMemberInfo.HasSameMetadataDefinitionAs(member))
                {
                    return myMemberInfo;
                }
            }

            throw new($"Member not found:{member.Name}");
        }

        //https://github.com/dotnet/runtime/blob/main/src/coreclr/System.Private.CoreLib/src/System/Reflection/MemberInfo.Internal.cs
        static bool HasSameMetadataDefinitionAs(this MemberInfo target, MemberInfo other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (target.MetadataToken != other.MetadataToken)
                return false;

            if (!target.Module.Equals(other.Module))
                return false;

            return true;
        }
    }
}