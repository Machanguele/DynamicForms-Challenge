import {InquiryCard} from "../../components/InquiryCard";
import React, {useEffect, useState} from "react";
import '../../styles/inquiry.css'
import {API_URL, apiCall} from "../../helpers/api";
import {MdAddToPhotos} from 'react-icons/md';
import {Button, Form, Modal, Alert} from "react-bootstrap";
import {useHistory} from "react-router-dom";

export const Inquiries = () => {
    const history = useHistory();
    const [inquiries, setInquiries] = useState([]);
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [description, setDescription] = useState("");
    const [showAlertSuccess, setShowAlertSuccess] = useState(false);
    const [showAlertError, setShowAlertError] = useState(false);
    const [isValid, setIsValid] = useState(true);



    useEffect( ()=>{
            apiCall(`${API_URL}/inquiries`, "GET", {}, false)
                .then( async ( response) => {
                    setInquiries(await  response.json())
                    setShowAlertSuccess(true)
                    //console.log(await response.json())
                })
                .catch((err) => {
                    console.log(err)
                });
        }
        ,[] )

    console.log(inquiries)


    function handleCreateInquiry(description){
        if(description != ""){
            setIsValid(true)
            apiCall(`${API_URL}/inquiries`, "POST", {description: description}, false)
                .then( async ( response) => {
                    setInquiries(await  response.json())
                    //console.log(await response.json())
                    setShowAlertSuccess(true);
                })
                .catch((err) => {
                    setShowAlertError(false);
                    console.log(err)
                });
        }else{setIsValid(false)}

    }

    return (
        <div id={'container'} className="row space-between justify-content-center overflow-auto">
            {
                (inquiries.length >0 ) &&
                inquiries.map(inquiry =>
                    <InquiryCard key={inquiry.id}
                                 description={inquiry.description}
                                 id={inquiry.id}
                                 createdAt={inquiry.creationDate}
                    />
                )
            }
            <Button id={"roundedButton"}
                //onClick={()=>{history.push(`/inquiry/create`)}}
                    onClick={()=>{setShowCreateModal(true)}}

            >
                <MdAddToPhotos size={25}/>
            </Button>

            <Modal
                size="lg"
                show={showCreateModal}
                onHide={() => setShowCreateModal(false)}
                aria-labelledby="example-modal-sizes-title-lg"
            >
                <Modal.Header closeButton>
                    <Modal.Title id="example-modal-sizes-title-lg" className={"justify-content-center"}>
                        Criar Inquérito
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group controlId="formBasicEmail">
                            <Form.Label>Descrição</Form.Label>
                            <Form.Control as="textarea"
                                          rows={3}
                                          value={description}
                                          onChange={event => setDescription(event.target.value)}
                                          placeholder="Descreva o Inquerito em poucas palavras" />
                            <Form.Text className="text-muted">
                                Descreva o inquerito em poucas palavras
                            </Form.Text>
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="info" onClick={() => setShowCreateModal(false)}>
                        Cancel
                    </Button>
                    <Button variant="primary" onClick={() =>
                                    handleCreateInquiry(description)}
                         >
                        Create Inquiry
                    </Button>
                </Modal.Footer>
            </Modal>

            <Alert show={showAlertSuccess} variant="success">
                <Alert.Heading>Mensagem de sucesso</Alert.Heading>
                <p>
                    Inquerito criado com sucesso! Adcione as questoes no proximo passo.
                </p>
                <hr />
                <div className="d-flex justify-content-end">
                    <Button onClick={() => setShowAlertSuccess(!showAlertSuccess)} variant="outline-success">
                        Fechar
                    </Button>
                </div>
            </Alert>

            <Alert show={showAlertError} variant="danger">
                <Alert.Heading>Mensagem de Erro</Alert.Heading>
                <p>
                    Falha ao criar Inquerito.
                </p>
                <hr />
                <div className="d-flex justify-content-end">
                    <Button onClick={() => setShowAlertError(!showAlertError)} variant="outline-success">
                        Fechar
                    </Button>
                </div>
            </Alert>

            <Alert show={!isValid} variant="danger">
                <Alert.Heading>Mensagem de Validacao</Alert.Heading>
                <p>
                    Verifique se digitou inormacoes validas.
                </p>
                <hr />
                <div className="d-flex justify-content-end">
                    <Button onClick={() => setIsValid(!isValid)} variant="outline-success">
                        Fechar
                    </Button>
                </div>
            </Alert>
        </div>
    );
};