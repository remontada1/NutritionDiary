using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WebApplication1.Identity
{
    public class PasswordHasher : IPasswordHasher
    {
        public virtual string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        /// <summary>
        ///     Verify that a password matches the hashedPassword
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        public virtual PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (Crypto.VerifyHashedPassword(hashedPassword, providedPassword))
            {
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }





        public interface IPasswordHasher
        {
            /// <summary>
            ///     Hash a password
            /// </summary>
            /// <param name="password"></param>
            /// <returns></returns>
            string HashPassword(string password);

            /// <summary>
            ///     Verify that a password matches the hashed password
            /// </summary>
            /// <param name="hashedPassword"></param>
            /// <param name="providedPassword"></param>
            /// <returns></returns>
            PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
        }





    }
}