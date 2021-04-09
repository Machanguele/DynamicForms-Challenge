// @flow
import {useParams} from 'react-router-dom';
import React, {useEffect, useState} from "react";
import {API_URL, apiCall} from "../../helpers/api";
import '../../styles/inquiryDetail.css';
import {Questions} from "../../components/Questions";
import {Button, Form, Modal} from "react-bootstrap";
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
    const [showCreateQuestion, setShowCreateQuestion] = useState(false);

    const params = useParams();



    useEffect(() => {
        let id = params.id;
        apiCall(`${API_URL}/inquiries/${id}`, "GET", {}, false)
            .then( async ( response) => {setSelectedInquiry(await  response.json())})
            .catch((err) => {console.log(err)});

        apiCall(`${API_URL}/inputTypes/`, "GET", {}, false)
            .then( async ( response) => {setInputTypes(await  response.json())})
            .catch((err) => {console.log(err)});

        apiCall(`${API_URL}/questionCategories`, "GET", {}, false)
            .then( async ( response) => {setQuestionCategories(await  response.json())})
            .catch((err) => {console.log(err)});

    }, [])


    console.log("Inquiries");
    console.log(selectedInquiry);
    console.log("Inputs");
    console.log(inputTypes);
    console.log("Categ");
    console.log(questionCategories);
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
                    onClick={()=>{setShowCreateQuestion(true)}}
            >
                Adicionar mais questoes
            </Button>

            {(selectedInquiry?.questionsDtos?.length >0) &&
            <Button id={"submitButton"}
                     variant={"info"}
                     onClick={() => {
                     }}
            >
                Enviar resposta
            </Button>}

            <Modal
                size="lg"
                show={showCreateQuestion}
                onHide={() => setShowCreateQuestion(false)}
                aria-labelledby="example-modal-sizes-title-lg"
            >
                <Modal.Header closeButton>
                    <Modal.Title id="example-modal-sizes-title-lg" className={"justify-content-center"}>
                        Adicionar questão
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group controlId="formBasicEmail">
                            <Form.Label>Descrição</Form.Label>
                            <Form.Control as="textarea"
                                          rows={3}
                                          //value={description}
                                          //onChange={event => setDescription(event.target.value)}
                                          //placeholder="Descreva o Inquerito em poucas palavras"
                            />
                            <Form.Text className="text-muted">
                                Descreva o inquerito em poucas palavras
                            </Form.Text>
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="warning" onClick={() => setShowCreateQuestion(false)}>
                        Cancelar
                    </Button>
                    <Button variant="info" onClick={() => {}}
                        //handleCreateInquiry(description)}
                    >
                        Adicionar
                    </Button>
                </Modal.Footer>
            </Modal>


        </div>
    );
};