terraform {
  backend "s3" {
    bucket = "docgen-levelup-bucket"
    key = "docgen/terraform.tfstate"  # Specify the path/key for your state file
    region = "eu-west-1"
  }
}