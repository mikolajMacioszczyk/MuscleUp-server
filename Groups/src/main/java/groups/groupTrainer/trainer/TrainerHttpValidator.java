package groups.groupTrainer.trainer;

import groups.common.innerCommunicators.AbstractHttpValidator;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;

@Service
public class TrainerHttpValidator extends AbstractHttpValidator implements TrainerValidator {

    private static final String GET_TRAINER_BY_ID_PATH = "auth/trainer/find/";


    @Override
    public HttpStatus checkTrainerId(UUID trainerId) {

        String path = concatenate(GET_TRAINER_BY_ID_PATH, trainerId.toString());

        return checkIfIdExist(path);
    }
}
