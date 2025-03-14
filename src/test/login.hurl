# GET https://{{hostname}}:5004/login
# [Options]
# insecure: true # TODO: Self signed certificate
# HTTP 200

POST https://{{hostname}}:5002/api/identity/login
# # Access-Control-Request-Headers: authorization,content-type
# # Access-Control-Request-Method: POST
# # Host: {{hostname}}:5002
# # Origin: https://{{hostname}}:5004
# # Referer: https://{{hostname}}:5004/
# # Sec-Fetch-Mode: cors
# # Sec-Fetch-Dest: empty
# # Sec-Fetch-Site: same-site
[Options]
insecure: true # TODO: Self signed certificate
{}

HTTP 204