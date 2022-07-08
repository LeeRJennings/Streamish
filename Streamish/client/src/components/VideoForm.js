import { useState, useEffect } from "react";
import { addVideo } from "../modules/videoManager";

export const VideoForm = ({ getVideos }) => {
    const [video, setVideo] = useState({
        title: "",
        url: "",
        description: ""
    })

    const handleControlledInputChange = (e) => {
        const newVideo = {...video}
        let selectedVal = e.target.value
        newVideo[e.target.id] = selectedVal
        setVideo(newVideo)
    }

    const handleClickSaveVideo = (e) => {
        e.preventDefault()
        if (video.title === "" || video.url === "") {
            window.alert('bruh, you gotta have a Title and URL. Description is optional.')
        } else {
            addVideo(video)
            getVideos()
        }

    }

    return (
        <form>
            <h4>Add a Video</h4>
            <fieldset>
                <div>
                    <label htmlFor="title">Title:  </label>
                    <input type="text" id="title" required onChange={handleControlledInputChange} value={video.title}/>
                </div>
            </fieldset>
            <fieldset>
                <div>
                    <label htmlFor="url">URL:  </label>
                    <input type="text" id="url" required onChange={handleControlledInputChange} value={video.url}/>
                </div>
            </fieldset>
            <fieldset>
                <div>
                    <label htmlFor="description">Description:  </label>
                    <input type="text" id="description" onChange={handleControlledInputChange} value={video.description}/>
                </div>
            </fieldset>
            <button type="button" onClick={handleClickSaveVideo}>Save Video</button>
        </form>
    )
}