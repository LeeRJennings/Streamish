import React, { useEffect, useState } from "react";
import { getAllVideos } from "../modules/videoManager";

const VideoList = () => {
  const [videos, setVideos] = useState([]);

  const getVideos = () => {
    getAllVideos()
    .then(videos => setVideos(videos));
  };

  useEffect(() => {
    getVideos();
  }, []);

  return (
    <div>
      {videos.map(v => 
        <div>{v.title}</div>
      )}
    </div>
  );
}

export default VideoList;