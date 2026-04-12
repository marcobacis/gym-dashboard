# Column Encryption Setup

This document explains how to setup and configure column encryption for the Gym Dashboard application.

## Overview

The Gym Dashboard uses **AES encryption** via the [SoftFluent EntityFrameworkCore.DataEncryption](https://github.com/SoftFluent/EntityFrameworkCore.DataEncryption) library to encrypt sensitive data at rest in the database.

Currently, the **Gym password** field is encrypted. This ensures that sensitive credentials stored in the database are protected and cannot be read directly, even if someone gains access to the database files.

## Encryption Keys

To enable encryption, you need to provide two cryptographic components:

- **Key**: An AES encryption key - must be 16, 24, or 32 characters long (corresponding to AES-128, AES-192, or AES-256)
- **Initialization Vector (IV)**: A randomized value for AES encryption - must be exactly 16 bytes

Both values must be **base64-encoded** when stored in configuration.

## Setup Steps

### 1. Generate Encryption Keys

You can generate secure random keys using any method. For example, using PowerShell:

```powershell
# Generate a 32-byte (256-bit) key
$key = [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))

# Generate a 16-byte IV
$iv = [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(16))

Write-Output "Key: $key"
Write-Output "IV: $iv"
```

Or using OpenSSL:

```bash
# Generate a 32-byte key
openssl rand -base64 32

# Generate a 16-byte IV
openssl rand -base64 16
```

### 2. Configure User Secrets (Development)

For local development, store the encryption keys in [ASP.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

```bash
dotnet user-secrets set "Encryption:Key" "your-base64-encoded-key"
dotnet user-secrets set "Encryption:InitializationVector" "your-base64-encoded-iv"
```

Or manually add to your secrets file:

```json
{
  "Encryption": {
    "Key": "your-base64-encoded-key",
    "InitializationVector": "your-base64-encoded-iv"
  }
}
```

### 3. Configure Production Environment

For production deployments, set the encryption keys via environment variables:

```bash
export Encryption__Key=your-base64-encoded-key
export Encryption__InitializationVector=your-base64-encoded-iv
```

Or in your deployment configuration (Docker, Kubernetes, etc.):

```dockerfile
ENV Encryption__Key=your-base64-encoded-key
ENV Encryption__InitializationVector=your-base64-encoded-iv
```

## How It Works

### Architecture

1. **EncryptionOptions** (`Persistence/EncryptionOptions.cs`): Configuration class that holds the Base64-encoded keys and provides byte array conversions
2. **AesProvider**: Encryption provider from SoftFluent that handles actual encryption/decryption operations
3. **Entity Configuration**: Marked properties are automatically encrypted before being saved and decrypted when retrieved

### Encrypted Fields

The following fields are currently encrypted:

| Entity | Field | Configuration |
|--------|-------|---------------|
| Gym | Password | `Configuration/Gym.Configuration.cs` - marked with `.IsEncrypted()` |

### Adding More Encrypted Fields

To encrypt additional fields:

1. Open the entity's configuration class (e.g., `Persistence/Configuration/Gym.Configuration.cs`)
2. Add `.IsEncrypted()` to the property builder:

```csharp
builder.Property(e => e.SensitiveField).IsEncrypted();
```

3. Run a database migration:

```bash
dotnet ef migrations add "Encrypt_SensitiveField" --project Persistence --startup-project WebApp
dotnet ef database update
```

> **Note**: Existing unencrypted data in the database will not be automatically encrypted. You'll need to write a migration to re-encrypt existing values or clear the data.

## Database Migrations

Encryption is automatically applied when the database context is initialized. The application runs pending migrations on startup:

```csharp
// In Program.cs
await app.Services.ApplyMigrations();
```

## Security Considerations

- **Key Storage**: Never commit encryption keys to version control. Always use User Secrets or environment variables
- **Key Rotation**: The current implementation does not support key rotation. Rotating keys requires re-encrypting all data
- **Connection Security**: Always use encrypted database connections (SSL/TLS) in production
- **Backup**: Store encryption keys separately from database backups for disaster recovery
- **Access Control**: Limit database access to authorized applications and users only

## Troubleshooting

**Error: "Key must be 16, 24, or 32 characters long"**
- Verify the Key is base64-encoded and decodes to exactly 16, 24, or 32 bytes
- Check that the key is properly set in User Secrets or environment variables

**Error: "Initialization Vector must be 16 bytes"**
- Ensure the IV is base64-encoded and decodes to exactly 16 bytes

**Encrypted data appears corrupted**
- Confirm the same Key and IV are being used for encryption and decryption
- Check that the database wasn't modified outside of the application

