# ✅ Automated UI Tests for Microsoft Teams (Playwright + C#)

This project contains automated UI tests for the Microsoft Teams web application.  
It was created as part of a QA Engineering task and demonstrates the use of:

- C# with Playwright
- MSTest as the test runner
- Docker for easy test execution in any environment
- GitHub Actions for CI test automation

---

## 🔧 Technologies Used

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Playwright for .NET](https://playwright.dev/dotnet/)
- MSTest
- Docker (for containerized testing)
- GitHub Actions (for CI)

---

## ▶️ How to Run Tests Locally

1. Install Playwright browsers (only needed once):

    ```bash
    pwsh bin/Debug/net8.0/playwright.ps1 install
    ```

2. Run the tests:

    ```bash
    dotnet test --logger:trx --results-directory TestResults
    ```

3. (Optional) Generate HTML report from results:

    ```bash
    dotnet tool install -g dotnet-reportgenerator-globaltool
    reportgenerator -reports:"TestResults/*.trx" -targetdir:"TestReport" -reporttypes:Html
    ```

---

## 🐳 How to Run Tests in Docker

1. Build the image:

    ```bash
    docker build -t playwright-tests .
    ```

2. Run tests in the container:

    ```bash
    docker run --rm -v "$(Get-Location)/TestResults:/app/TestResults" playwright-tests
    ```

---

## ⚙️ Continuous Integration (CI) and Project Structure

This project includes a simple GitHub Actions workflow that:

- Builds the Docker image
- Runs the tests inside the container
- Uploads test result artifacts (.trx)

You can find the workflow file here:
`.github/workflows/ci.yml`

PlaywrightTests/

├── Pages/ # Page Object Model classes

├── Tests/ # Test methods

├── Base/ # Base test setup

├── Resources/Downloads/ # Folder for downloaded files

├── Dockerfile # Docker test environment

└── .github/workflows/ # CI workflow

## Final Note

Thank you for the opportunity to work on this assignment.  
I enjoyed building it and learning new things along the way.  
I would really appreciate any feedback or suggestions for improvement.

Kind regards,  
Jan Mlčák

