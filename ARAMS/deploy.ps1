sam package --profile default --template-file template.yml --s3-bucket elgin-sg-builds --output-template-file packaged.yml

aws cloudformation deploy --template-file packaged.yml --stack-name ARAMS  --capabilities CAPABILITY_IAM --profile default --region ap-southeast-1