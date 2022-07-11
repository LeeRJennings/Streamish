import React from "react";
import { Routes, Route } from "react-router-dom";
import { UserVideos } from "./UserVideos";
import VideoDetails from "./VideoDetails";
import { VideoForm } from "./VideoForm";
import VideoList from "./VideoList";


const ApplicationViews = () => {
  return (
    <Routes>
      <Route path="/" >
        <Route index element={<VideoList/>} />
        <Route path="videos">
          <Route index element={<VideoList/>} />
          <Route path="add" element={<VideoForm/>} />
          <Route path=":id" element={<VideoDetails/>} />
        </Route>
        <Route path="users">
            <Route path=":id" element={<UserVideos/>} />
        </Route>
      </Route>
      <Route path="*" element={<p>Whoops, nothing here...</p>} />
    </Routes>
  );
};

export default ApplicationViews;