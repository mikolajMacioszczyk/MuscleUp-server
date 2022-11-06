package content.bodyPart.service;

import content.bodyPart.controller.form.BodyPartForm;
import content.bodyPart.entity.BodyPart;
import content.bodyPart.entity.BodyPartFactory;
import content.bodyPart.repository.BodyPartRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class BodyPartService {

    private final BodyPartRepository bodyPartRepository;
    private final BodyPartFactory bodyPartFactory;


    @Autowired
    public BodyPartService(BodyPartRepository bodyPartRepository) {

        Assert.notNull(bodyPartRepository, "bodyPartRepository must not be null");

        this.bodyPartRepository = bodyPartRepository;
        this.bodyPartFactory = new BodyPartFactory();
    }


    public UUID saveBodyPart(BodyPartForm bodyPartForm) {

        Assert.notNull(bodyPartForm, "bodyPartForm must not be null");

        BodyPart bodyPart = bodyPartFactory.create(bodyPartForm);

        return bodyPartRepository.save(bodyPart);
    }

    public UUID updateBodyPart(UUID id, BodyPartForm bodyPartForm) {

        Assert.notNull(bodyPartForm, "bodyPartForm must not be null");

        BodyPart bodyPart = bodyPartRepository.getById(id);

        bodyPart.update(bodyPartForm.name());

        return bodyPartRepository.update(bodyPart);
    }

    public void deleteBodyPart(UUID id) {

        Assert.notNull(id, "id must not be null");

        bodyPartRepository.delete(id);
    }
}
