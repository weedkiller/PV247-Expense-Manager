using ExpenseManager.Business.Utilities.BadgeCertification.BadgeCertifiers;

namespace ExpenseManager.Business.Utilities.BadgeCertification
{
    /// <summary>
    /// Contract for Badge Certifier Resolver which manages all implementations of IBadgeCertifier
    /// </summary>
    public interface IBadgeCertifierResolver
    {
        /// <summary>
        /// Gets instance of badge certifier according to the badge name
        /// </summary>
        /// <param name="badgeName">The name of the badge to find certifier for</param>
        /// <returns>Badge certifier with corresponding name or null, if not found</returns>
        BadgeCertifier ResolveBadgeCertifier(string badgeName);
    }
}