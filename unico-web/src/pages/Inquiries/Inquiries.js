import {InquiryCard} from "../../components/InquiryCard";
import React, {useEffect, useState} from "react";
import '../../styles/inquiry.css'
import {API_URL, apiCall} from "../../helpers/api";

export const Inquiries = () => {

    const [inquiries, setInquiries] = useState([]);

    useEffect( ()=>{
          apiCall(`${API_URL}/inquiries`, "GET", {}, false)
            .then((response) => {
                 setInquiries( response.json())
            })
            .catch((err) => {
                console.log(err)
            });
    }
    ,[] )

    console.log(inquiries)

    return (
        <div id={'container'} className="d-inline-flex p-2 space-between justify-content-center">
            <InquiryCard description={"Formulario de analise de satisfacao dos clientes"} />
            <InquiryCard  description={"Formulario de taxas"}/>
            <InquiryCard  description={"formulario"}/>
        </div>
    );
};