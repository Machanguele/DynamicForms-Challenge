import React from "react";

const createFormData = (pickedImages, command) => {


    const files = new FormData();
    if(command != ""){
        files.append(
            "command",
            JSON.stringify(command)
        );
    }

    if (pickedImages.length > 0) {

        pickedImages.map((image) => {
            const lastOccurance = image.uri.lastIndexOf("/");

            const getFileName = image.uri.slice(
                lastOccurance + 1,
                image.uri.length
            );

            files.append("files", {
                name: getFileName,
                type: "image/jpg",
                uri:image.uri
            });
        });
    }

    return files;
};

export default createFormData;