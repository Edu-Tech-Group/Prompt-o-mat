$certname = Read-Host "Please enter the name of the certificate"
$certdomain = Read-Host "Please enter the DNS domain for the certificate"
$certpassword = Read-Host "Please enter the password for the certificate" -AsSecureString

$now = Get-Date
$expiryDate = $Now.AddMonths(24)
Write-Host "Generating certificate"
$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject "CN=$certname, O=Edu-Tech, C=US" -DNSName $certdomain -KeyUsage DigitalSignature -FriendlyName "Prompt-o-mat" -CertStoreLocation "Cert:\CurrentUser\My" -KeyAlgorithm RSA -KeyLength 4096 -NotBefore $now -NotAfter $expiryDate
Write-Host "Exporting certificate file"
Export-PfxCertificate -Cert $cert -FilePath "./$certname.pfx" -Password $certpassword

Write-Host "Cleaning up..."
$thumbprint = $cert.Thumbprint
Remove-Item -Path "Cert:\CurrentUser\My\$thumbprint" -DeleteKey

# for dev: base64-encode the file, so we can save it in GH Actions
# [System.Convert]::ToBase64String((Get-Content .\prompt-o-mat.pfx -AsByteStream)) | Out-File prompt-o-mat-b64.txt