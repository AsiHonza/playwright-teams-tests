FROM mcr.microsoft.com/dotnet/sdk:8.0

# Install browser dependencies (this fixes the error)
RUN apt-get update && apt-get install -y \
    libglib2.0-0 \
    libnspr4 \
    libnss3 \
    libdbus-1-3 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libxcb1 \
    libxkbcommon0 \
    libatspi2.0-0 \
    libx11-6 \
    libxcomposite1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxrandr2 \
    libgbm1 \
    libcairo2 \
    libpango-1.0-0 \
    libasound2 \
    && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /app

# Copy csproj and restore
COPY PlaywrightTests.csproj ./
RUN dotnet restore

# Copy the rest
COPY . ./
RUN dotnet build --no-restore

# Install Playwright CLI + browsers
RUN dotnet tool install --global Microsoft.Playwright.CLI \
    && export PATH="$PATH:/root/.dotnet/tools" \
    && playwright install

CMD ["dotnet", "test", "--logger:trx", "--results-directory", "TestResults"]
