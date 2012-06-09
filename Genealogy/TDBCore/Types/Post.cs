using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel.Syndication;
using System.ComponentModel;

namespace TDBCore.Types
{
    [DataContract]
    public class Post
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Uri Uri { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime PostedOn { get; set; }

        [DataMember(IsRequired=false)]
        public DateTime ModifiedOn { get; set; }

        private bool _hasExcerpt = false;

        [DataMember(IsRequired=false)]
        public bool HasExcerpt
        {
            get { return _hasExcerpt; }
            set { _hasExcerpt = value; }
        }

        [DataMember(IsRequired=false)]
        public string Excerpt { get; set; }

        public void Update(Post post)
        {
            if (post == null)
            {
                return;
            }

            this.Author = post.Author;
            this.Title = post.Title;
            this.Content = post.Content;
            this.HasExcerpt = post.HasExcerpt;
            this.Excerpt = post.Excerpt;
            this.ModifiedOn = DateTime.Now;
        }

        public static explicit operator SyndicationItem(Post post)
        {
            if (post == null)
            {
                return null;
            }

            SyndicationItem item = new SyndicationItem();
            item.Authors.Add(new SyndicationPerson { Name = post.Author });
            item.Title = SyndicationContent.CreatePlaintextContent(post.Title);
            item.PublishDate = post.PostedOn;
            item.LastUpdatedTime = post.ModifiedOn;
            item.Id = "post-" + post.Id.ToString();
            item.Content = SyndicationContent.CreateHtmlContent(post.Content);
            if (post.HasExcerpt)
            {
                item.Summary = SyndicationContent.CreateHtmlContent(post.Excerpt);
            }

            return item;
        }
    }

    [DataContract]
    public class PostResultSet
    {
        private List<Post> _posts = null;

        [DataMember]
        public List<Post> Posts
        {
            get
            {
                if (_posts == null)
                {
                    _posts = new List<Post>();
                }

                return _posts;
            }
            set
            {
                _posts = value;
            }
        }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public int Start { get; set; }
    }
}
