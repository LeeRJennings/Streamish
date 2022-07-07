using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, Email, ImageUrl, DateCreated
                                        FROM UserProfile";
                    using (var reader = cmd.ExecuteReader())
                    {
                        var userProfiles = new List<UserProfile>();
                        while (reader.Read())
                        {
                            userProfiles.Add(new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            });
                        }
                        return userProfiles;
                    }
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, Email, ImageUrl, DateCreated
                                        FROM UserProfile
                                        WHERE Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        UserProfile userProfile = null;
                        if (reader.Read())
                        {
                            userProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            };
                        }
                        return userProfile;
                    }
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile (Name, Email, ImageUrl, DateCreated)
                                        OUTPUT INSERTED.ID
                                        VALUES (@Name, @Email, @ImageUrl, @DateCreated)";
                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE UserProfile
                                        SET Name = @Name,
                                            Email = @Email,
                                            ImageUrl = @ImageUrl,
                                            DateCreated = @DateCreated
                                        WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@Id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM UserProfile
                                        WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public UserProfile GetByIdWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT up.Id, up.Name, up.Email, up.ImageUrl, up.DateCreated AS UserDateCreated,
                                               
                                               v.Id AS VideoId, v.Title, v.Description, v.Url, v.DateCreated AS VideoDateCreated,

                                               c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserId
                                        
                                        FROM UserProfile up
                                        JOIN Video v ON v.UserProfileId = up.Id
                                        LEFT JOIN Comment c ON c.VideoId = v.Id
                                        WHERE up.Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        UserProfile userProfile = null;

                        while(reader.Read())
                        {
                            var userId = DbUtils.GetInt(reader, "Id");

                            if (userProfile == null)
                            {
                                userProfile = new UserProfile()
                                {
                                    Id = userId,
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserDateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                    Videos = new List<Video>()
                                };
                            }
                            if (DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                var videoId = DbUtils.GetInt(reader, "VideoId");

                                var existingVideo = userProfile.Videos.FirstOrDefault(x => x.Id == videoId);
                                if (existingVideo == null)
                                {
                                    existingVideo = new Video()
                                    {
                                        Id = videoId,
                                        Title = DbUtils.GetString(reader, "Title"),
                                        Description = DbUtils.GetString(reader, "Description"),
                                        Url = DbUtils.GetString(reader, "Url"),
                                        DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                                        UserProfileId = userId,
                                        Comments = new List<Comment>(),
                                    };

                                    userProfile.Videos.Add(existingVideo);
                                }
                                if (DbUtils.IsNotDbNull(reader, "CommentId"))
                                {
                                    existingVideo.Comments.Add(new Comment()
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentId"),
                                        Message = DbUtils.GetString(reader, "Message"),
                                        VideoId = videoId,
                                        UserProfileId = DbUtils.GetInt(reader, "CommentUserId")
                                    });
                                }
                            }
                        }
                        return userProfile;
                    }
                }
            }
        }
    }
}
