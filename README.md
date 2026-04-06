# PartnerTransactionHub
A small demo app

# Partner Transactions BFF (.NET 5)

## 🧩 Overview

This project implements a Backend-for-Frontend (BFF) microservice that:

* Accepts partner transaction requests
* Validates incoming payloads
* Verifies partner identity via external API (mocked)
* Publishes validated transactions to RabbitMQ
* Provides resilience using Polly (retry strategy)

---

## 🏗 Architecture

* ASP.NET Core Web API (.NET 5)
* RabbitMQ (message broker)
* FluentValidation (input validation)
* Polly (resilience)
* Docker + docker-compose (local environment)

---

## 🔄 Flow

Client → API → Validation → Partner Verification → RabbitMQ → Legacy System

---

## ⚙️ Running the project

### 1. Start services (Docker)

```bash
cd src
docker-compose up --build
```

### 2. Access

* API: http://localhost:5000
* RabbitMQ UI: http://localhost:15672

---

## 🧪 Running tests

```bash
dotnet test
```

### Code coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutput=./coverage/ /p:CoverletOutputFormat=cobertura
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage/report
```

---

## 🔐 Security

The API is secured using an API Key mechanism:

Header:

```
X-API-KEY: super-secret-key
```

---

## ⚠️ Notes

* HTTPS is enabled for external access only but it's faulty currently
* Internal service communication uses HTTP
* RabbitMQ is used for asynchronous processing

---

## 🚀 Future improvements

* JWT authentication / OAuth2 for API Security
* Observability (OpenTelemetry) for API Security and Auditing

---
