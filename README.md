# Certificate for HTTPS

This project requires HTTPS to work with gRPC. For development, you can create a self-signed certificate and configure it for the project.

## Steps to Create the Certificate

1. **Create a Configuration File for the Certificate**:
   Create a file named `openssl-san.cnf` with the following content:
   ```plaintext
   [req]
   distinguished_name = req_distinguished_name
   req_extensions = req_ext
   x509_extensions = v3_ca
   prompt = no

   [req_distinguished_name]
   CN = localhost

   [req_ext]
   subjectAltName = @alt_names

   [v3_ca]
   subjectAltName = @alt_names

   [alt_names]
   DNS.1 = localhost
   ```

2. **Generate a Private Key and Certificate**:
   Run the following command:
   ```bash
   openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
       -keyout Certificates/server.key \
       -out Certificates/server.crt \
       -config openssl-san.cnf
   ```

3. **Create a `.pfx` File**:
   Combine the private key and certificate into a `.pfx` file:
   ```bash
   openssl pkcs12 -export \
       -out Certificates/localhost.pfx \
       -inkey Certificates/server.key \
       -in Certificates/server.crt \
       -password pass:yourpassword
   ```

4. **Configure the Project**:
   - Place the certificate in the `Certificates` directory.
   - Ensure the `docker-compose.yml` file has the correct configuration:
     ```yaml
     services:
       currency-service:
         environment:
           - Kestrel__Certificates__Default__Path=/app/Certificates/localhost.pfx
           - Kestrel__Certificates__Default__Password=yourpassword
         volumes:
           - ./Certificates:/app/Certificates:ro
     ```

5. **Restart the Containers**:
   Run the following commands:
   ```bash
   docker compose down
   docker compose up --build
   ```

## Notes

- The certificate is valid for 365 days. After it expires, it must be renewed.
- For production, use certificates signed by a trusted Certificate Authority (CA).
