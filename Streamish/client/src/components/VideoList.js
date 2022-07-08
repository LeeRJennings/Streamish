import React, { useEffect, useState } from "react";
import Video from "./Video";
import { VideoForm } from "./VideoForm";
import { getAllVideos, getAllVideosWithComments, searchVideos } from "../modules/videoManager";

const VideoList = () => {
  const [videos, setVideos] = useState([]);

  const getVideos = () => {
    getAllVideosWithComments()
    .then(videos => setVideos(videos));
  };

  const handleVideoSearch = (e) => {
    if (e.keyCode === 13) {
      searchVideos(e.target.value)
      .then(videos => setVideos(videos))
    }
  }

  useEffect(() => {
    getVideos();
  }, []);

  return (
    <>
    <VideoForm getVideos={getVideos}/>
    <br/>
    <div className="container">
      <h4>Search Videos</h4>
      <input type="text" id="videoSearch" placeholder="search videos here..." onKeyUp={handleVideoSearch} />
      <div className="row justify-content-center">
        {videos.map((video) => (
          <Video video={video} key={video.id} />
        ))}
      </div>
    </div>
    </>
  );
};

export default VideoList;