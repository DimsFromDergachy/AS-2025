
POST https://103.90.72.212:5002/api/identity/login
Host: 103.90.72.212:5002
User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:135.0) Gecko/20100101 Firefox/135.0
Accept: */*
Accept-Language: en-US,en;q=0.5
Accept-Encoding: gzip, deflate, br, zstd
Access-Control-Request-Method: POST
Access-Control-Request-Headers: content-type
Referer: https://103.90.72.212:5004/
Origin: https://103.90.72.212:5004
Connection: keep-alive
Sec-Fetch-Dest: empty
Sec-Fetch-Mode: cors
Sec-Fetch-Site: same-site
Priority: u=4
{"email":"{{username}}","password":"{{password}}"}
HTTP 204


# POST https://{{hostname}}:5004/api/identity/login
#     {"email":"WRONG_USER","password":"{{password}}"}
#     HTTP 401

# POST https://{{hostname}}:5004/api/identity/login
#     {"email":"{{username}}","password":"WRONG_PASSWORD"}
#     HTTP 401

# POST https://{{hostname}}:5004/api/identity/login
#     {"email":"{{username}}","password":"{{password}}"}
#     HTTP 200

