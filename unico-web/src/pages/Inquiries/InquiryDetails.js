// @flow
import {useParams} from 'react-router-dom';
import React, {useEffect, useState} from "react";
import {API_URL, apiCall} from "../../helpers/api";
import '../../styles/inquiryDetail.css';
import {Questions} from "../../components/Questions";
import {Button, Form, Modal, Alert, Spinner} from "react-bootstrap";
import {FiPlus} from "react-icons/fi";
import createFormData from "../../helpers/CreateFormData";



export const InquiryDetails = () => {
    const [inputTypes, setInputTypes] = useState([]);
    const [selectedInquiry, setSelectedInquiry] = useState();
    const [questionCategories, setQuestionCategories] = useState([]);
    const [questions, setQuestions] = useState([]);

    const [title, setTitle] = useState("");
    const [isRequired, setIsRequired] = useState(true);
    const [inputTypeId, setInputTypeId] = useState();
    const [selectedInputType, setSelectedInputType] = useState("");
    const [questionCategoryId, setQuestionCategoryId] = useState();
    const [questionOptions, setQuestionOptions] = useState([]);
    const [auxOption, setAuxOption] = useState("");
    const [showCreateQuestion, setShowCreateQuestion] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

    const[files, setFiles]= useState([])
    const [previewImages, setPreviewImage] = useState([])

    const params = useParams();

    useEffect(()=>{
        let aux = inputTypes.filter(x=>x.id == inputTypeId);
        setSelectedInputType(aux[0])
        /*if(selectedInputType?.description === "resposta Fechada"){
            setQuestionOptions(["Sim", "Nao"])
        }else{
            setQuestionOptions([])
        }*/
    }, [inputTypeId])


    useEffect(() => {
        let id = params.id;
        dispatchHelper("inquiry", `inquiries/${id}`, "GET", {})
        dispatchHelper("questionCategories", `questionCategories`, "GET", {})
        dispatchHelper("inputType", `inputTypes`, "GET", {})
    }, [isLoading])


    function  handleSelectedImage(event){
        if(!event.target.files){
            return;
        }
        const selectedImages = Array.from(event.target.files)
        setFiles(selectedImages)

        const selectedImagesPreview = selectedImages.map(image=>{
            return URL.createObjectURL(image);
        })
        setPreviewImage(selectedImagesPreview);
    }

    function handleSubmitInquiry(){
        let id = params.id;

        apiCall(`${API_URL}/inquiries/submit/${id}`, "POST", {"inquiryId": id}, false)
            .then( async ( response) => {
                console.log( await response.json)
                setIsLoading(!isLoading)
            })
            .catch((err) => {console.log(err)
            });
    }

    async function  handleCreateQuestion(){

        if(title != "" && inputTypeId !="" && questionCategoryId !=""){
            let id = params.id;
            let data = {
                "title":title,
                "isRequired": isRequired,
                "inputTypeId": inputTypeId,
                "questionCategoryId":  questionCategoryId,
                "questionOptions": questionOptions,
                "inquiryId": id
            }

            const formData = createFormData(files, data)

            apiCall(`${API_URL}/questions/create`, "POST", {
                "title":title,
                "isRequired": isRequired,
                "inputTypeId": inputTypeId,
                "questionCategoryId":  questionCategoryId,
                "questionOptions": questionOptions,
                "inquiryId": id

            }, false)
                .then( async ( response) => {
                    console.log( await response.json)
                    setIsLoading(!isLoading)
                    //getInquiries()
                    setQuestionOptions([])
                    setShowCreateQuestion(false)

                })
                .catch((err) => {console.log(err)
                });
        }
    }

    function dispatchHelper(resource, url, method, body){
        apiCall(`${API_URL}/${url}`, method, body, false)
            .then( async ( response) => {
                resource ==="questionCategories"?
                    setQuestionCategories(await  response.json())
                    : resource === "inquiry" ?
                    setSelectedInquiry(await  response.json())
                    : setInputTypes(await  response.json())
            })
            .catch((err) => {
                console.log(err)});
    }

    return (
        <>
            { selectedInquiry ?
                <div id={"container"}>
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
                        {selectedInquiry?.questionsDtos?.length > 0 &&
                        <div>
                            {
                                selectedInquiry?.questionsDtos.map(question =>
                                    <Questions question={question}/>)
                            }
                        </div>
                        }
                    </div>

                    {(selectedInquiry?.questionsDtos?.length >0) && (!selectedInquiry?.submitted) &&
                    <Button id={"submitButton"}
                            variant={"success"}
                            onClick={handleSubmitInquiry()}
                    >
                        Submeter Questionario
                    </Button>}


                    {(!selectedInquiry?.submitted) &&
                    <Button id={"addMore"}
                            variant={"info"}
                            onClick={() => {
                                setShowCreateQuestion(true)
                            }}
                    >
                        Adicionar mais Questões
                    </Button>
                    }


                    <Modal
                        size="lg"
                        show={showCreateQuestion}
                        onHide={() => setShowCreateQuestion(false)}
                        aria-labelledby="example-modal-sizes-title-lg"
                    >
                        <Modal.Header closeButton>
                            <Modal.Title id="example-modal-sizes-title-lg" className={"justify-content-center"}>
                                Adicionando e configurando Questões para o Inquérito
                            </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <Form>
                                <Form.Group>
                                    <Form.Label>Titulo</Form.Label>
                                    <Form.Control type="text"
                                                  placeholder="pergunta a ser respondida"
                                                  value={title}
                                                  data-toggle="toggle"
                                                  onChange={event => setTitle(event.target.value)}
                                    />
                                </Form.Group>

                                <Form.Group>
                                    <div className="form-group form-check">
                                        <input type="checkbox"
                                               className="form-check-input"
                                               id="exampleCheck1"
                                               value={isRequired}
                                               onChange={()=>setIsRequired(!isRequired)}
                                        />
                                            <label className="form-check-label"
                                                   htmlFor="exampleCheck1"
                                            >
                                                Resposta Obrigatória *
                                            </label>
                                    </div>
                                </Form.Group>


                                <Form.Group>
                                    <div className="input-block">
                                        <label htmlFor="images">Fotos</label>

                                        <div className="images-container">
                                            {previewImages.map(image => {
                                                return (
                                                    <img src={image} alt={"name"} width={100}/>
                                                )

                                            })}
                                            <label htmlFor="image[]" className="new-image">
                                                <FiPlus size={24} color="#15b6d6"/>
                                            </label>

                                            <input
                                                multiple
                                                type="file"
                                                id="image[]"
                                                onChange={()=>handleSelectedImage()}
                                            />
                                        </div>
                                    </div>
                                </Form.Group>

                                <Form.Group>
                                    <Form.Label>Categoria</Form.Label>
                                    {
                                        (questionCategories?.length > 0)
                                        &&
                                        <Form.Control
                                            as="select"
                                            className="mr-sm-2"
                                            id="inlineFormCustomSelect"
                                            custom
                                            onChange={event => setQuestionCategoryId(event.target.value)}
                                        >
                                            <option value={""}>Escolha a categoria</option>
                                            {
                                                questionCategories?.map(op =>
                                                    <option value={op.id}
                                                        //onChange={event =>setQuestionCategoryId(event.target.value)}
                                                    >
                                                        {op.description}</option>
                                                )
                                            }
                                        </Form.Control>
                                    }
                                </Form.Group>

                                <Form.Group>
                                    <Form.Label>Especifique o tipo de cammpo de resposta</Form.Label>
                                    {
                                        (inputTypes?.length > 0)
                                        &&
                                        <Form.Control
                                            as="select"
                                            className="mr-sm-2"
                                            id="inlineFormCustomSelect"
                                            custom
                                            onChange={event => setInputTypeId(event.target.value)}
                                        >
                                            {
                                                inputTypes?.map(op =>
                                                    <option value={op.id}>
                                                        {op.description}</option>
                                                )
                                            }
                                        </Form.Control>
                                    }
                                </Form.Group>

                                {
                                    (selectedInputType?.description != "Resposta Aberta") &&

                                    <Form.Group>
                                        <Form.Label>Opções da questão </Form.Label>

                                        {questionOptions.length >0
                                        &&
                                        <Form.Text>
                                            {
                                                questionOptions.map(function (option) {
                                                    return <p>{option}</p>;
                                                })
                                            }
                                        </Form.Text>
                                        }

                                        {/*{selectedInputType?.description != "Resposta Aberta" &&
                                        <>*/}
                                            <Form.Control type="text"
                                                          placeholder="Introduza a opcao"
                                                          value={auxOption}
                                                          onChange={event => setAuxOption(event.target.value)}
                                            />
                                            <Button variant="info"
                                                    onClick={() => questionOptions.push(auxOption)}
                                                    className={"m-3"}
                                            >
                                                Adicionar Opcao
                                            </Button>
                                        {/*</>
                                        }*/}
                                    </Form.Group>
                                }


                            </Form>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="warning" onClick={() => setShowCreateQuestion(false)}>
                                Cancelar
                            </Button>
                            <Button variant="info" onClick={() => handleCreateQuestion()}
                            >
                                Adicionar
                            </Button>
                        </Modal.Footer>
                    </Modal>
                </div>:
                <Spinner animation="border" variant="info" />
            }
        </>
    );
};