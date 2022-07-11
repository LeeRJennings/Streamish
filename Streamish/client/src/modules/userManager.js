const baseUrl = '/api/UserProfile'

export const getUserWithVideos = (id) => {
    return fetch(`${baseUrl}/GetWithVideos/${id}`)
        .then((res) => res.json())
  }