import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getUserWithVideos } from "../modules/userManager";
import Video from "./Video";
import { Link } from "react-router-dom";

export const UserVideos = () => {
    const [user, setUser] = useState({
        name: "",
        email: "",
        videos: []
    })
    const { id } = useParams()

    useEffect(() => {
        getUserWithVideos(id).then(user => setUser(user))
    }, [])

    if (!user) {
        return null;
      }
    
    return (
    <div className="container">
        <div className="row justify-content-center">
            <h1>{user.name}</h1>
            <h4>{user.email}</h4>
            <div className="col-sm-12 col-lg-6">
                {user.videos?.map((video) => (
                    <div key={'video__'+video.id}>
                        <iframe className="video"
                        src={video.url}
                        title="YouTube video player"
                        frameBorder="0"
                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                        allowFullScreen />
                
                        <p>
                            <Link to={`/videos/${video.id}`}>
                                <strong>{video.title}</strong>
                            </Link>
                        </p>
                        <p>{video.description}</p>
                        <p><u>Comments:</u></p>
                        <>{video.comments?.map((c) => (<p key={'comment__'+c.id}>{c.message}</p>))}</>
                    </div>
                ))}
            </div>
        </div>
    </div>
    );
};
    
export default UserVideos;