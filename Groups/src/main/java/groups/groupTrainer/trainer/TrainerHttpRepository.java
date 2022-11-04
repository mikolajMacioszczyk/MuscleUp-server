package groups.groupTrainer.trainer;

import groups.common.innerCommunicators.AbstractHttpRepository;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;

@Service
public class TrainerHttpRepository extends AbstractHttpRepository implements TrainerRepository {

    private static final String GET_TRAINER_BY_ID_PATH = "auth/trainer/find/";

    private final TrainerFactory trainerFactory;


    public TrainerHttpRepository() {

        this.trainerFactory = new TrainerFactory();
    }


    @Override
    public HttpStatus checkTrainerId(UUID trainerId) {

        Assert.notNull(trainerId, "trainerId must not be null");

        String path = concatenate(GET_TRAINER_BY_ID_PATH, trainerId.toString());

        return checkIfIdExist(path);
    }

    @Override
    public Trainer getTrainerById(UUID trainerId) {

        Assert.notNull(trainerId, "trainerId must not be null");

        String path = concatenate(GET_TRAINER_BY_ID_PATH, trainerId.toString());

        String jsonTrainer = getById(path);

        return trainerFactory.create(jsonTrainer);
    }
}
