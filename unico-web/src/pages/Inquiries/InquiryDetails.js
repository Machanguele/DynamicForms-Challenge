// @flow
import * as React from 'react';
import {useParams} from 'react-router-dom';


export const InquiryDetails = () => {
    const params = useParams();

    console.log(params.id);
    return (
        <div>
            Inquire Details {params.id}
        </div>
    );
};