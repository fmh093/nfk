# NFK Application - Deliberately Vulnerable Webshop

The NFK Application is a simple webshop that is deliberately vulnerable for educational purposes.

It contains many types of vulnerabilities and weaknesses, and is aimed at the principles of the [OWASP Top 10 (2021)](https://owasp.org/www-project-top-ten/).

**Mac users**
Checkout branch *mac*

You will be prompted for your keychain password in order to install the certificate.

You might also need to run the following command:
dotnet dev-certs https --trust

🚨 **Please make sure your firewall is enabled.** or **Turn off your internet connection.**

---

## Getting Started

You can run the application with IIS Express. 

Here are some ways to approach your exploration of the NFK Application:

### Blackbox Testing
Imagine you're a bad actor intent on exploiting any vulnerability in the webshop.
- Use the shop and monitor network requests, URLs, etc.
- If you find something suspicious, dive into the code to investigate further.
- Try to exploit anything you find.

### Whitebox Testing
Approach the application as if you were conducting a secure code review.
- Be alert for anything that looks suspicious.
- Attempt to exploit these issues within the shop.

If you're unsure where to start, the [OWASP Top 10](https://owasp.org/www-project-top-ten/) can offer some guidance on what to look for.

Once you've identified a vulnerability or weakness, think about how you might fix these issues.

---

### Database Reset Instructions
To reset the database:
- Go to `Program.cs`.
- Comment out the line `if (IsDatabaseInitialized()) return;`

This will reset most data back to their default values once you run the application. If needed, a backup is available in `nfk_backup.db`.
