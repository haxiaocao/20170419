using Elasticsearch.Net;
using ES.Sample.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//referenct: https://github.com/elastic/elasticsearch-net

namespace ES.Sample.ClientManager
{
    public class ClientManager
    {
        ElasticClient m_client = null;

        public ElasticClient GetESClient()
        {
            if (m_client != null) return m_client;

            ElasticClient client = null;

            ////connect to a single node.
            //var node = new Uri("http://myserver:9200");
            //var settings = new ConnectionSettings(node);
            //client = new ElasticClient(settings);

            ////using a connection pool
            var nodes = new Uri[]
            {
	            new Uri("http://myserver1:9200"),
	            new Uri("http://myserver2:9200"),
	            new Uri("http://myserver3:9200")
            };

            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            client = new ElasticClient(settings);

            return client;
        }

        public void Index()
        {
            if (m_client != null)
            {
                var tweet = new Tweet
                {
                    Id = 1,
                    User = "kimchy",
                    PostDate = new DateTime(2009, 11, 15),
                    Message = "Trying out NEST, so far so good?"
                };

                var response = m_client.Index(tweet, idx => idx.Index("mytweetindex")); //or specify index via settings.DefaultIndex("mytweetindex");
                //var response = client.IndexAsync(tweet, idx => idx.Index("mytweetindex")); // returns a Task<IndexResponse>
            }
        }

        public void GetDocument()
        {
            if (m_client != null)
            {
                var response = m_client.Get<Tweet>(1, idx => idx.Index("mytweetindex")); // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
                var tweet = response.Source; // the original document
            }
        }

        public void SearchForDocs()
        {
            if (m_client != null)
            {
                ////method 1 
                //var response = m_client.Search<Tweet>(s => s
                //                .From(0)
                //                .Size(10)
                //                .Query(q =>
                //                        q.Term(t => t.User, "kimchy")
                //                        || q.Match(mq => mq.Field(f => f.User).Query("nest"))
                //                    )
                //                );


                // method 2, on the condition the method one can not satisfy you.
                var request = new SearchRequest
                {
                    From = 0,
                    Size = 10,
                    Query = new TermQuery { Field = "user", Value = "kimchy" }
                        || new MatchQuery { Field = "description", Query = "nest" }
                };

                var response = m_client.Search<Tweet>(request);

                
                ////Method 3 : .LowLevel is of type IElasticLowLevelClient
                //var response = m_client.LowLevel.Search("myindex", "elasticsearchprojects", new
                //{
                //    from = 0,
                //    size = 10,
                //    fields = new[] { "id", "name" },
                //    query = new
                //    {
                //        term = new
                //        {
                //            name = new
                //            {
                //                value = "NEST",
                //                boost = 2.0
                //            }
                //        }
                //    }
                //});
            }
        }


    }
}
