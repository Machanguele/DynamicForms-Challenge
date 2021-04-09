export const API_URL = "http://localhost:5000/api";


export const apiCall = (url, method, body, fileFlag) => {

    if (method !== "GET") {
        if (fileFlag) {
            return fetch(url, {
                method: method,
                headers: {
                    "Content-Type": "multipart/form-data"
                    //Authorization: `Bearer ${token}`,
                },
                body: body,
            });
        }

        return fetch(url, {
            method: method,
            headers: {
                "Content-Type": "application/json",
                //Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify(body),
        });
    }

    return fetch(url, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            //Authorization: `Bearer ${token}`,
        },
        //body: JSON.stringify(body),
    });
};


