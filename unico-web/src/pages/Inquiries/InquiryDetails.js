// @flow
import {useParams} from 'react-router-dom';
import React, {useEffect, useState} from "react";
import {API_URL, apiCall} from "../../helpers/api";
import '../../styles/inquiryDetail.css';
import {Questions} from "../../components/Questions";
import {Button, Form, Modal, Alert} from "react-bootstrap";
import {MdAddToQueue} from "react-icons/md";
import {FiPlus} from "react-icons/fi";
import apiAxios from "../../helpers/apiAxios";


export const InquiryDetails = () => {
    const [inputTypes, setInputTypes] = useState([]);
    const [selectedInquiry, setSelectedInquiry] = useState();
    const [questionCategories, setQuestionCategories] = useState([]);
    const [questions, setQuestions] = useState([]);

    const [title, setTitle] = useState("");
    const [isRequired, setIsRequired] = useState(false);
    const [inputTypeId, setInputTypeId] = useState();
    const [selectedInputType, setSelectedInputType] = useState("");
    const [questionCategoryId, setQuestionCategoryId] = useState();
    const [questionOptions, setQuestionOptions] = useState([]);
    const [auxOption, setAuxOption] = useState("");
    const [showCreateQuestion, setShowCreateQuestion] = useState(false);

    const[files, setFiles]= useState([])
    const [previewImages, setPreviewImage] = useState([])

    const params = useParams();



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

    async function  handleCreateQuestion(){

        if(title != "" && inputTypeId !="" && questionCategoryId !=""){

            apiCall(`${API_URL}/inquiries/`, "POST", {
                "title":title,
                "isRequired": isRequired,
                "inputTypeId": inputTypeId,
                "questionCategoryId":  questionCategoryId,
                "questionOptions": questionOptions,
                "inquiryId": selectedInquiry.id

            }, false)
                .then( async ( response) => {
                    console.log(response)
                    getInquiries(selectedInquiry.id)
                   // Alert.al("New Question Added")
                })
                .catch((err) => {console.log(err)
                   // Alert("Fail to add Question")
                });
        }
    }


    useEffect(()=>{
        let aux = inputTypes.filter(x=>x.id == inputTypeId);
        setSelectedInputType(aux[0])
        if(selectedInputType?.description === "resposta Fechada"){
            setQuestionOptions(["Sim", "Nao"])
        }else{
            (setQuestionOptions([]))
        }
    }, [inputTypeId])


    useEffect(() => {
        let id = params.id;
        getInquiries(id);
        getInputTypes();
        getQuestionCategories();
    }, [])

    function getInquiries(id){
        apiCall(`${API_URL}/inquiries/${id}`, "GET", {}, false)
            .then( async ( response) => {setSelectedInquiry(await  response.json())})
            .catch((err) => {console.log(err)});
    }

    function getInputTypes(){
        apiCall(`${API_URL}/inputTypes/`, "GET", {}, false)
            .then( async ( response) => {setInputTypes(await  response.json())})
            .catch((err) => {console.log(err)});
    }

    function getQuestionCategories(){
        apiCall(`${API_URL}/questionCategories`, "GET", {}, false)
            .then( async ( response) => {setQuestionCategories(await  response.json())})
            .catch((err) => {console.log(err)});
    }



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
                Adicionar mais Questões
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
                                          onChange={event => setTitle(event.target.value)}
                            />
                        </Form.Group>


                        <Form.Group>
                            <div className="input-block">
                                <label htmlFor="images">Fotos</label>

                                <div className="images-container">

                                    {previewImages.map(image=>{
                                        return(
                                            <img src={image} alt={"name"} width={100}/>
                                        )

                                    })}
                                    <label htmlFor="image[]" className="new-image">
                                        <FiPlus size={24} color="#15b6d6" />
                                    </label>

                                    <input
                                        multiple
                                        type="file"
                                        id="image[]"
                                        onChange={handleSelectedImage}
                                    />

                                </div>
                            </div>
                        </Form.Group>

                        <Form.Group>
                            <Form.Label>Categoria</Form.Label>
                            {
                                (questionCategories?.length> 0)
                                &&
                                <Form.Control
                                    as="select"
                                    className="mr-sm-2"
                                    id="inlineFormCustomSelect"
                                    custom
                                    onChange={event =>setQuestionCategoryId(event.target.value)}
                                >
                                    {
                                        questionCategories?.map(op=>
                                            <option value={op.id}
                                                /* onChange={event =>setQuestionCategoryId(event.target.value)}*/
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
                                (inputTypes?.length> 0)
                                &&
                                <Form.Control
                                    as="select"
                                    className="mr-sm-2"
                                    id="inlineFormCustomSelect"
                                    custom
                                    onChange={event =>setInputTypeId(event.target.value)}
                                >
                                    {
                                        inputTypes?.map(op=>
                                            <option value={op.id}>
                                                {op.description}</option>
                                        )
                                    }
                                </Form.Control>
                            }
                        </Form.Group>

                        {(selectedInputType?.description != "Resposta Aberta" )&&
                        <Form.Group>
                            <Form.Label>Opções da questão  </Form.Label>

                            <Form.Text>
                                {
                                    questionOptions.map(function(option) {
                                        return <p>{option}</p>;
                                    })
                                }
                            </Form.Text>

                            {selectedInputType?.description != "resposta Fechada" &&
                                <>
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
                                </>
                               }
                        </Form.Group>
                        }




                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="warning" onClick={() => setShowCreateQuestion(false)}>
                        Cancelar
                    </Button>
                    <Button variant="info" onClick={handleCreateQuestion()}
                        //handleCreateInquiry(description)}
                    >
                        Adicionar
                    </Button>
                </Modal.Footer>
            </Modal>


        </div>
    );
};