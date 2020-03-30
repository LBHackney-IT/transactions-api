provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}

data "aws_iam_role" "ec2_container_service_role" {
  name = "ecsServiceRole"
}

data "aws_iam_role" "ecs_task_execution_role" {
  name = "ecsTaskExecutionRole"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-development-apis"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/transactions-api/state"
  }
}

module "development" {
  source                      = "github.com/LBHackney-IT/aws-hackney-components-per-service-terraform.git//modules/environment/backend/fargate"
  cluster_name                = "development-apis" # Replace with your cluster name.
  ecr_name                    = "hackney/transactions-api"
  environment_name            = "development"
  application_name            = "transactions-api"    # Replace with your application name.
  security_group_name         = "transactions-api" # Replace with your security group name, WITHOUT SPECIFYING environment .
  vpc_name                    = "vpc-development-apis"
  host_port                   = 1002
  port                        = 1002
  desired_number_of_ec2_nodes = 2
  lb_prefix                   = "nlb-development-apis"
  ecs_execution_role          = data.aws_iam_role.ecs_task_execution_role.arn
  lb_iam_role_arn             = data.aws_iam_role.ec2_container_service_role.arn
  task_definition_environment_variables = {
    ASPNETCORE_ENVIRONMENT = "development"
  }
  cost_code = "B0811"
  task_definition_environment_variable_count = 2

  task_definition_secrets      = {}
  task_definition_secret_count = 0
}

/*   ADD ANY OTHER CUSTOM RESOURCES REQUIRED HERE      */
