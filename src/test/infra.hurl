
# MINIO
POST http://{{hostname}}:9001/minio/health/live
HTTP 200

# PG Admin
GET http://{{hostname}}:5050/login
HTTP 200

#Scalar
GET https://localhost:5002/scalar/
[Options]
insecure: true # TODO: Self signed certificate
HTTP 200