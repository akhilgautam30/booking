# Booker API Test Suite

## Description
The **Booker API Test Suite** is a comprehensive testing framework designed to validate the functionality of the [Restful Booker API](https://restful-booker.herokuapp.com). It includes automated tests for creating, retrieving, updating, and deleting bookings, ensuring the API adheres to expected behavior. The suite is built using .NET 8.0, NUnit, and Reqnroll, with integrated reporting via Allure.


---

## Assumptions

While writing the tests for the **Booker API**, the following assumptions were made:

1. **Pre-loaded Data**: The API comes with many pre-loaded data.  These records reset themselves every 10 minutes back to their default state, So response can be dynamic.

2. **Bug Exploration**: The API is intentionally loaded with a variety of bugs for you to explore. However, it is important to keep in mind that some behaviors that appear to be bugs might actually be the intended behavior of the application.

---

## Table of Contents
1. Features
2. Installation
3. Usage
4. Running the Project
5. Dependencies
6. Generating Reports
7. CI/CD Pipeline
8. Contact

---

## Features
- **Create Booking**: Tests for creating bookings with valid and invalid data.
- **Retrieve Booking**: Tests for retrieving booking details by ID.
- **Update Booking**: Tests for updating existing bookings.
- **Retrieve BookingIDs**: Tests for retrieving booking ID.
- **Edge Cases**: Tests for handling missing fields, invalid data, and boundary conditions.
- **Integrated Reporting**: Generates detailed test execution reports using Allure.

---

## Installation

### Prerequisites
1. **.NET SDK**: Install the [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
2. **Node.js and npm**: Install [Node.js](https://nodejs.org/) (includes npm).
3. **Java Runtime**: Install [OpenJDK 17](https://openjdk.org/projects/jdk/17/).
4. **Allure CLI**: Install Allure Commandline for generating reports.

   ```bash
   npm install -g allure-commandline --save-dev
   ```

5. **Docker**: Install Docker for containerized builds and testing.

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/BookerApiTest.git
   cd BookerApiTest
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build --configuration Release
   ```

---

## Usage

### Running Tests
To execute the test suite, use the following command:
```bash
dotnet test --logger "trx;LogFileName=test-results.trx"
```

### Generating Reports
1. After running the tests, generate the Allure report:
    ```bash
    allure generate allure-results -o allure-report --clean
    ```
    > **Note**: Sometimes, the `allure-results` directory may be generated in `bin/Debug/net8.0/`. Ensure you navigate to the correct path before generating the report.

2. Serve the report on a local server:
    ```bash
    allure serve allure-results
    ```
    or

    ```bash
    allure serve bin/Debug/net8.0/allure-report
    ```
   ```

---

### Running with Docker
1. Build the Docker image:
   ```bash
   docker build -t booker-api-test .
   ```

2. Run the tests inside the container:
   ```bash
   docker run --rm booker-api-test dotnet test --logger "trx;LogFileName=test-results.trx"
   ```

3. Copy the results to your local machine:
   ```bash
   docker cp <container_id>:/app/BookerApiTest/allure-results ./allure-results
   ```

4. Generate and serve the report:
   ```bash
   allure serve allure-results
   ```

---

## Dependencies

### Main Dependencies
- **.NET 8.0 SDK**: For building and running the project.
- **NUnit**: Test framework for writing and executing tests.
- **Reqnroll**: For BDD-style test execution.
- **RestSharp**: For making HTTP requests to the API.
- **Allure CLI**: For generating detailed test reports.

### Additional Dependencies
- **Java Runtime (OpenJDK 17)**: Required for Allure report generation.
- **Node.js and npm**: For installing Allure CLI.

---

## CI/CD Pipeline

### GitLab CI/CD Configuration
The project includes a GitLab CI/CD pipeline for automated builds, tests, and report generation. Below is the `.gitlab-ci.yml` configuration:

```yaml
stages:
  - build_and_test

build_and_test:
  stage: build_and_test
  image: docker:24.0.5
  services:
    - docker:dind
  variables:
    DOCKER_DRIVER: overlay2
  script:
    - docker build -t booker-api-test:latest -f BookerApiTest/Dockerfile .
    - mkdir -p ./bin ./TestResults
    - docker run --rm -v $CI_PROJECT_DIR/bin:/app/BookerApiTest/bin booker-api-test:latest dotnet build
    - docker run --rm -v $CI_PROJECT_DIR/bin:/app/BookerApiTest/bin -v $CI_PROJECT_DIR/TestResults:/app/BookerApiTest/TestResults booker-api-test:latest dotnet test --logger "trx;LogFileName=test-results.trx" || true
    - mkdir -p ./allure-results
    - cp -r ./bin/Debug/net8.0/allure-results/* ./allure-results/ || true
    - docker run --rm -v $CI_PROJECT_DIR/allure-results:/allure-results -v $CI_PROJECT_DIR/allure-report:/allure-report booker-api-test:latest allure generate -o allure-report --clean
  artifacts:
    paths:
      - bin/
      - TestResults/
      - allure-results/
      - allure-report/
```

### Pipeline Breakdown
1. **Stages**: Defines a single stage `build_and_test`.
2. **Docker Image**: Uses `docker:24.0.5` with Docker-in-Docker (`docker:dind`) for containerized builds.
3. **Variables**: Configures `DOCKER_DRIVER` to `overlay2` for Docker performance.
4. **Script**:
   - Builds the Docker image for the project.
   - Runs the build and test commands inside the container.
   - Copies test results and Allure results to the appropriate directories.
   - Generates the Allure report.
5. **Artifacts**: Stores build outputs, test results, and reports for later use.

---

## Contact
For questions or support, please contact:

- **Name**: Mayuri Todkar
