Create a bucket
----
Please refer the [offocial S3 guide](https://aws.amazon.com/ko/documentation/s3/).

Add a bucket policy
----
You may add a policy on your bucket for [public access to indexfile](https://github.com/pjc0247/BuildsFlight/blob/master/doc/api.md).
```json
{
  "Version":"2012-10-17",
  "Statement":[
    {
      "Sid":"AddPerm",
      "Effect":"Allow",
      "Principal": "*",
      "Action":["s3:GetObject"],
      "Resource":["arn:aws:s3:::YOUR_BUCKET_NAME/buildsflight_index.json"]
    }
  ]
}
```
