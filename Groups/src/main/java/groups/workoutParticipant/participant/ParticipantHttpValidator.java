package groups.workoutParticipant.participant;

import groups.common.innerCommunicators.AbstractHttpValidator;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.UUID;

import static groups.common.utils.StringUtils.concatenate;

@Service
public class ParticipantHttpValidator extends AbstractHttpValidator implements ParticipantValidator {

    private static final String GET_PARTICIPANT_BY_ID_PATH = "carnets/gympass/";


    @Override
    public HttpStatus checkGympassId(UUID gympassId) {

        String path = concatenate(GET_PARTICIPANT_BY_ID_PATH, gympassId.toString());

        return checkIfIdExist(path);
    }
}
