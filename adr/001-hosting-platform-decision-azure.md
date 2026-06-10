# ADR-001: Hosting Platform Decision for CloudOrder REST API in Azure

## Status

Accepted

## Date

2026-06-10

## Decision Owners

CloudOrder Architecture Team

---

# 1. Context

CloudOrder is a cloud-native REST API application built using:

- .NET 8 Web API
- C#
- Entity Framework Core
- Azure SQL Database
- REST-based API architecture
- CI/CD deployment pipeline

The application requires an Azure hosting platform that provides:

- Reliable API hosting
- Production-grade availability
- Scalability
- Security
- Monitoring
- Easy deployment
- Low operational overhead

The platform should also support future evolution toward a distributed microservices architecture.

---

# 2. Problem Statement

We need to select an Azure hosting platform for deploying the CloudOrder REST API.

The selected platform should balance:

- Development speed
- Cost
- Operational complexity
- Scalability
- Enterprise readiness

---

# 3. Decision Drivers

The following factors are considered:

## Technical Requirements

- Support .NET 8 runtime
- Support REST API workloads
- Support secure configuration management
- Support monitoring and diagnostics
- Support automated deployment

## Operational Requirements

- Minimal infrastructure management
- High availability
- Easy scaling
- Easy troubleshooting

## Future Requirements

- Support containerization
- Support microservice evolution
- Support cloud-native practices

---

# 4. Options Considered

## Option 1: Azure App Service

### Overview

Azure App Service is a Platform-as-a-Service (PaaS) offering for hosting web applications and REST APIs.

Architecture:
    Client
    |
    |
    Azure API Management (Optional)
    |
    |
    Azure App Service
    |
    |
    CloudOrder REST API
    |
    |
    Azure SQL Database


---

## Advantages

### 1. Native .NET Support

Provides first-class support for:

- ASP.NET Core
- .NET 8
- Entity Framework Core

---

### 2. Simple Deployment

Supports:

- GitHub Actions
- Azure DevOps Pipeline
- ZIP deployment
- Container deployment

Deployment flow:
    Developer
    |
    Git Commit
    |
    CI/CD Pipeline
    |
    Azure App Service


---

### 3. Built-in Production Features

Provides:

- HTTPS
- SSL certificates
- Custom domains
- Authentication
- Application settings
- Managed Identity

---

### 4. Scaling Support

Supports:

## Vertical Scaling

Increase App Service Plan size.

Example:
    Basic
    |
    Standard
    |
    Premium

## Horizontal Scaling

Increase instances:
    App Instance 1
    App Instance 2
    App Instance 3

---

### 5. Deployment Slots

Supports zero downtime deployment.

Example:
Production Slot

    <swap>

Staging Slot

New version can be deployed and tested before production release.

---

## Disadvantages

- Less infrastructure control compared to Kubernetes
- Vendor-managed runtime environment
- Large-scale microservices may require migration later

---

# Option 2: Azure Container Apps

## Overview

Serverless container hosting platform.

Architecture:
    Docker Image
    |
    Azure Container Registry
    |
    Azure Container Apps
    |
    CloudOrder API

---

## Advantages

- Cloud-native approach
- Container-based deployment
- Auto scaling
- Supports microservices
- Lower operational overhead compared to Kubernetes

---

## Disadvantages

- Requires Docker knowledge
- More deployment complexity
- Possible cold start when scaling from zero

---

## Suitable For

Future microservice deployment.

Example:
    Order Service
    Payment Service
    Customer Service

---

# Option 3: Azure Kubernetes Service (AKS)

## Overview

Managed Kubernetes platform.

Architecture:
    Container
    |
    AKS Cluster
    |
    Pods
    |
    Services


---

## Advantages

- Enterprise-grade orchestration
- Maximum scalability
- Supports complex microservice systems
- Industry standard container platform

---

## Disadvantages

- High complexity
- Requires Kubernetes expertise
- Requires cluster administration
- Higher operational cost

---

## Suitable For

Large-scale platforms with many services.

---

# Option 4: Azure Functions

## Overview

Serverless compute platform.

---

## Advantages

- Pay only for execution
- Automatic scaling
- Good for event-driven workloads

---

## Disadvantages

- Not ideal for primary REST API
- Cold start issues
- Execution limitations
- Complex APIs become difficult to maintain

---

## Suitable For

Background processing:

Example:
    Order Created Event
    |
    Azure Function
    |
    Send Email

---

# Option 5: Azure Virtual Machine

## Overview

Traditional VM hosting.

Architecture:
    VM
    |
    IIS
    |
    .NET API  

---

## Advantages

- Full control
- Any software can be installed

---

## Disadvantages

Requires management of:

- OS updates
- Security patches
- Scaling
- Availability

---

## Suitable For

Legacy applications.

---

# 5. Decision Matrix

| Criteria | App Service | Container Apps | AKS | Functions | VM |
|---|---|---|---|---|---|
| .NET Support | Excellent | Excellent | Excellent | Good | Good |
| Deployment | Easy | Medium | Complex | Easy | Medium |
| Scaling | Good | Excellent | Excellent | Excellent | Manual |
| Cost | Medium | Low-Medium | High | Low | Medium |
| Operations | Low | Medium | High | Low | High |
| Microservice Ready | Medium | High | Very High | Medium | Low |

---

# 6. Decision

## Selected Platform

Azure App Service

---

# 7. Decision Reasoning

Azure App Service is selected because it provides the best balance between:

- Simplicity
- Reliability
- Cost
- Scalability
- Production readiness

CloudOrder currently has:

- Single REST API
- Moderate expected traffic
- Need for fast delivery
- Need for enterprise deployment practices

App Service avoids unnecessary operational complexity while providing production-grade capabilities.

---

# 8. Consequences

## Positive Consequences

- Faster development cycle
- Simple deployment process
- Built-in monitoring
- Reduced infrastructure management
- Easy scaling
- Secure configuration support

---

## Negative Consequences

- Less control compared to Kubernetes
- Possible future migration required for very large workloads

---

# 9. Future Evolution Path

CloudOrder can evolve in stages.

## Phase 1: Current
Azure App Service

  |

CloudOrder API

  |

Azure SQL
---

## Phase 2: Container Based


Docker Image

  |

Azure Container Apps

  |

CloudOrder Services


---

## Phase 3: Enterprise Microservices


AKS Cluster

|
+-- Order Service
|
+-- Payment Service
|
+-- Customer Service
|
+-- Notification Service


---

# 10. Rejected Alternatives

| Alternative | Reason |
|---|---|
| Azure Functions | Designed mainly for event-driven workloads |
| Azure VM | Too much infrastructure management |
| AKS | Over-engineered for current stage |
| Container Apps | Reserved for future containerized architecture |

---

# 11. Implementation Plan

Azure resources required:

- Azure App Service Plan
- Azure App Service
- Application Insights
- Azure Key Vault
- Managed Identity
- Azure SQL Database

Deployment flow:
Developer

|

Git Repository

|

CI/CD Pipeline

|

Azure App Service

|

Production API

---

# Final Decision Summary

CloudOrder REST API will be hosted on Azure App Service.

This decision provides a production-ready, scalable, and maintainable hosting model while keeping a clear migration path toward Azure Container Apps or AKS as the system grows.