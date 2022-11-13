using System.Runtime.CompilerServices;

namespace Common.BaseClasses
{
    public abstract class ConcurrentTokenHandlerBase
    {
        private static HashSet<string> ProcessedTokens = new HashSet<string>();
        protected static int WaitMiliseconds = 100;

        #region Concurrency

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected bool LockToken(string invitationId)
        {
            if (ProcessedTokens.Contains(invitationId))
            {
                return false;
            }
            ProcessedTokens.Add(invitationId);
            return true;
        }

        protected void ReleaseToken(string invitationId)
        {
            if (ProcessedTokens.Contains(invitationId))
            {
                ProcessedTokens.Remove(invitationId);
            }
        }

        #endregion
    }
}
