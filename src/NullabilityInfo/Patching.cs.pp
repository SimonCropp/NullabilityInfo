#nullable enable

namespace System.Reflection
{
    internal static partial class NullabilityInfoExtensions
    {
        internal static MemberInfo GetMemberWithSameMetadataDefinitionAs(this Type type, MemberInfo member)
        {
            const BindingFlags all = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            foreach (MemberInfo info in type.GetMembers(all))
            {
                if (info.HasSameMetadataDefinitionAs(member))
                {
                    return info;
                }
            }

            throw new MissingMemberException(type.FullName, member.Name);
        }

        //https://github.com/dotnet/runtime/blob/main/src/coreclr/System.Private.CoreLib/src/System/Reflection/MemberInfo.Internal.cs
        static bool HasSameMetadataDefinitionAs(this MemberInfo target, MemberInfo other)
        {
            if (target.MetadataToken != other.MetadataToken)
                return false;

            if (!target.Module.Equals(other.Module))
                return false;

            return true;
        }

        //https://github.com/dotnet/runtime/issues/23493
        internal static bool IsGenericMethodParameter(this Type target)
        {
            return target.IsGenericParameter && target.DeclaringMethod != null;
        }
    }
}