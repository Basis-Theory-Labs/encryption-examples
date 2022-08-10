# Encryption Examples

[![Verify](https://github.com/Basis-Theory-Labs/encryption-examples/actions/workflows/verify.yml/badge.svg)](https://github.com/Basis-Theory-Labs/encryption-examples/actions/workflows/verify.yml)

These examples were developed to show a progressively more complex encryption implementation to fully implement the [NIST Key Management Lifecycle](https://csrc.nist.gov/CSRC/media/Events/Key-Management-Workshop-2001/documents/lifecycle-slides.pdf).

The final example demonistrates a middleware approach to implementing multiple KMS and encryption schemes to be able to dynamically encrypt and decrypt data with minimal code.

## Using the Examples
### Dependencies
- [Docker](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://www.docker.com/products/docker-desktop)
- [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)

### Build the Examples

The provided script will check for all dependencies and build the solution.

Run the following command from the root of the project:

```sh
make verify
```

## Example 1 - No Encryption

This example demonstrates a banking application that does not implement any data encryption of bank account details.

To run the example, run the following command:

```sh
make example-01
```

In your browser, navigate to [https://localhost:7034/banks](https://localhost:7034/banks)

## Example 2 - Basic Encryption

This example demonstrates a banking application that has implemented basic AES encryption with a static encryption key.

To run the example, run the following command:

```sh
make example-02
```

In your browser, navigate to [https://localhost:7034/banks](https://localhost:7034/banks)

## Example 3 - Shared Encryption

This example demonstrates a banking application that has multiple applications that need access to the same encryption key. In this example we have implemented a KMS to house and provide secure access to the key.

To run the example, run the following command:

```sh
make example-03
```

In your browser, navigate to [http://localhost:5990/banks](http://localhost:5990/banks)

## Example 4 - Key Rotation

This example demonstrates a banking application that has to implement key rotation requirements. This shows how to set expiration date on the key so we automatically generate a new key when the existing key expires. This example is limited to payload size since we are just using RSA encryption key from the KMS.

To run the example, run the following command:

```sh
make example-04
```

In your browser, navigate to [http://localhost:5890/banks](http://localhost:5890/banks)

## Example 5 - Open KMS

This example demonstrates a banking application that wants to simplify their encryption and KMS provider options. We implement the [Basis Theory](https://basistheory.com/) Open KMS SDK to be able to define schemes which sets encryption algorithm, handler, and key options to provide a generic and extensible abstraction away from any encryption pattern and KMS provider.

To run the example, run the following command:

```sh
make example-05
```

In your browser, navigate to [http://localhost:5790/banks](http://localhost:5790/banks)
