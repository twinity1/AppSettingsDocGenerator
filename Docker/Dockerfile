# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY run.sh run.sh
ENTRYPOINT ["bash", "run.sh"]