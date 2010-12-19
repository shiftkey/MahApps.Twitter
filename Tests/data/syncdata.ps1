[IO.Directory]::SetCurrentDirectory((Convert-Path (Get-Location -PSProvider FileSystem)))
$request = [System.Net.WebRequest]::Create("http://api.twitter.com/1/statuses/public_timeline.json")
$response = $request.GetResponse()
$requestStream = $response.GetResponseStream()
$readStream = new-object System.IO.StreamReader $requestStream
new-variable db
$db = $readStream.ReadToEnd()
$readStream.Close()
$response.Close()

$sw = new-object system.IO.StreamWriter(".\statuses\public_timeline.txt")
$sw.writeline($db)
$sw.close()
