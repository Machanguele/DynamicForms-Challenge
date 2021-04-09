// @flow
import {useParams} from 'react-router-dom';
import React, {useEffect, useState} from "react";
import {API_URL, apiCall} from "../../helpers/api";
import '../../styles/inquiryDetail.css';
import {Questions} from "../../components/Questions";
import {Button} from "react-bootstrap";
import {MdAddToQueue} from "react-icons/md";


export const InquiryDetails = () => {
    const [inputTypes, setInputTypes] = useState([]);
    const [selectedInquiry, setSelectedInquiry] = useState();
    const [questionCategories, setQuestionCategories] = useState([]);
    const [questions, setQuestions] = useState([]);

    const [title, setTitle] = useState("");
    const [isRequired, setIsRequired] = useState(false);
    const [inputTypeId, setInputTypeId] = useState();
    const [questionCategoryId, setQuestionCategoryId] = useState();
    const [files, setFiles] = useState([]);
    const [questionOptions, setQuestionOptions] = useState([]);

    const params = useParams();



    useEffect(() => {
        let id = params.id;
        apiCall(`${API_URL}/inquiries/${id}`, "GET", {}, false)
            .then( async ( response) => {setSelectedInquiry(await  response.json())})
            .catch((err) => {console.log(err)});
    }, [])


    console.log("Inquiries");
    console.log(selectedInquiry);
    return (
        <div id={"container"} >
            <div id={"HeaderContainer"} className={"text-white"}>
                <div id={"inquiry"}>
                    <h4>Inquérito</h4>
                    <hr></hr>
                    <div>
                        <h6>Descrição</h6>
                        <p>{selectedInquiry?.description}</p>
                    </div>
                </div>
            </div>

            <div id={"questionsContainer"}>
                {selectedInquiry?.questionsDtos?.length >0 &&
                <div>
                    {
                        selectedInquiry?.questionsDtos.map(question =>
                            <Questions question={question} />)
                    }
                </div>
                }
            </div>

            <Button id={"addMore"}
                    variant={"secondary"}
                    onClick={()=>{}}
            >
                Adicionar mais questoes
            </Button>

            <Button id={"submitButton"}
                    variant={"info"}
                    onClick={()=>{}}
            >
               Enviar resposta
            </Button>
        </div>
    );
};