using System;

namespace JAVS_VENDOR.ImageServices
{
    
        public class AWSConfiguration : IAWSConfiguration
        {
            public AWSConfiguration()
            {
                BucketName = "vendor-buck";
                Region = "ap-south-1";
                AwsAccessKey = "AKIAVFM7XVXHWELHNLXR";
                AwsSecretAccessKey = "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";

            }

            public string BucketName { get; set; }
            public string Region { get; set; }
            public string AwsAccessKey { get; set; }
            public string AwsSecretAccessKey { get; set; }

        }
    
}

